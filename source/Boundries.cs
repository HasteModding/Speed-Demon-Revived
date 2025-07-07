using Landfall.Modding;
using UnityEngine;

namespace SpeedDemon
{
    [LandfallPlugin]
    public class SpeedDemonCeiling
    {
        public static float softHeight = 180f;
        public static float hardHeight = 240f;
        public static float rayHeightAdjustment = 400f; // Make sure no rays start clipping into ground
        public static float minimumMapHeight = -150f;
        public static float rayRadius = 250f;
        public static float timeSinceSoftLerp = 10f;
        public static float maxSoftLerpFrequency = 0.025f;

        static SpeedDemonCeiling()
        {
            Debug.Log("[SpeedDemon] SpeedDemonCeiling class initializing...");
            On.GM_Run.Update += (orig, self) =>
            {
                // If player is too high, force them to start diving a bit
                RaycastHit[] rayHitMap = GetGroundRays(PlayerCharacter.localPlayer.transform.position, 32, rayRadius, 3000f);
                bool isAboveGround = IsAboveGround(rayHitMap, 0.45f);
                float distanceAboveGround = DistanceAboveGround(rayHitMap, isAboveGround);
                if (distanceAboveGround > hardHeight)
                {
                    Vector3 newVelocity = EnsureDownwardVelocity(PlayerCharacter.localPlayer.refs.rig.velocity, -2f);
                    PlayerCharacter.localPlayer.refs.rig.velocity = newVelocity;
                    //Debug.Log($"[SpeedDemon] Player hit hard ceiling, setting velocity vector to {newVelocity}");
                    //Debug.Log($"[SpeedDemon] isAboveGround is {isAboveGround}");
                    //Debug.Log($"[SpeedDemon] distanceAboveGround is {distanceAboveGround}");
                }
                if (distanceAboveGround > softHeight && timeSinceSoftLerp > maxSoftLerpFrequency)
                {
                    timeSinceSoftLerp = 0f;
                    Vector3 newVelocity = LerpedLerpVelocity(
                        PlayerCharacter.localPlayer.refs.rig.velocity,
                        PlayerCharacter.localPlayer.refs.rig.position,
                        0f,
                        0.05f,
                        -10f
                    );
                    PlayerCharacter.localPlayer.refs.rig.velocity = newVelocity;
                    //Debug.Log($"[SpeedDemon] Player hit soft ceiling, setting velocity vector to {newVelocity}");
                    //Debug.Log($"[SpeedDemon] isAboveGround is {isAboveGround}");
                    //Debug.Log($"[SpeedDemon] distanceAboveGround is {distanceAboveGround}");
                }
                timeSinceSoftLerp += Time.deltaTime;
                orig(self);
            };
        }

        public static bool rayHitGround(RaycastHit hit)
        {
            return hit.collider != null && hit.point.y >= minimumMapHeight;
        }

        private static RaycastHit[] GetGroundRays(Vector3 position, int peripheralRayCount, float checkRadius, float raycastDistance)
        {
            Vector3 adjustedPosition = new Vector3(position.x, position.y + rayHeightAdjustment, position.z); // Make sure no rays start clipping into ground
            RaycastHit[] hits = new RaycastHit[peripheralRayCount];

            // Peripheral rays
            for (int i = 0; i < peripheralRayCount; i++)
            {
                float angle = i * Mathf.PI * 2f / peripheralRayCount;
                Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * checkRadius;
                Vector3 rayOrigin = adjustedPosition + offset;

                Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastDistance);
                hits[i] = hit;
            }
            return hits;
        }

        private static bool IsAboveGround(RaycastHit[] rayHitMap, float minNullChainPercent)
        {
            RaycastHit[] peripheralRaysDoubled = new RaycastHit[rayHitMap.Length * 2];
            Array.Copy(rayHitMap, 0, peripheralRaysDoubled, 0, rayHitMap.Length); // Copy the array twice so it can loop back on itself
            Array.Copy(rayHitMap, 0, peripheralRaysDoubled, rayHitMap.Length, rayHitMap.Length);
            int maxNullChain = 0;
            int currentNullChain = 0;
            // Loop through array and find the longest chain of nulls (longest segment of the circle that is empty
            foreach (RaycastHit hit in peripheralRaysDoubled)
            {
                if (!rayHitGround(hit))
                {
                    currentNullChain++;
                    if (currentNullChain > maxNullChain)
                        maxNullChain = currentNullChain;
                }
                else currentNullChain = 0;
            }
            // Return true if less than the minimum amount had no hit
            if (maxNullChain / (float)rayHitMap.Length < minNullChainPercent) return true;
            else return false;
        }

        private static float DistanceAboveGround(RaycastHit[] rayHitMap, bool isAboveGround)
        {
            if (isAboveGround)
            {
                int nonNullRays = 0;
                float combinedDistance = 0f;
                foreach (RaycastHit hit in rayHitMap)
                {
                    if (rayHitGround(hit))
                    {
                        nonNullRays++;
                        combinedDistance += hit.distance - rayHeightAdjustment;
                    }
                }
                return combinedDistance / nonNullRays;
            }
            else
            {
                if (!rayHitMap.Any(item => rayHitGround(item))) // Can't see the the ground
                {
                    return 0f; // May need changing, but this is here in case something goes horribly wrong and they're outside the playable area
                }
                else // Ground is visible, but player is above the void
                {    // Set roof from highest point to give player some leniency to get back into the map
                    float minDistance = float.MaxValue;
                    foreach (RaycastHit hit in rayHitMap)
                    {
                        if (rayHitGround(hit) && hit.distance - rayHeightAdjustment < minDistance)
                        {
                            minDistance = hit.distance - rayHeightAdjustment;
                        }
                    }
                    return minDistance;
                }
            }
        }

        private static Vector3 LerpedLerpVelocity(Vector3 velocity, Vector3 position, float minLerp, float maxLerp, float maxPitch)
        {
            if (velocity == Vector3.zero) return velocity;
            // Decide how much to lerp by how far into the soft ceiling they are
            float softCeilingPercent = (position.y - softHeight) / (hardHeight - softHeight);
            float lerpAmount = Mathf.Lerp(minLerp, maxLerp, softCeilingPercent);

            float magnitude = velocity.magnitude;
            Vector3 direction = velocity.normalized;

            // Convert to spherical angles
            float pitch = Mathf.Asin(direction.y) * Mathf.Rad2Deg; // vertical angle
            float yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // Lerp pitch if needed
            if (pitch > maxPitch)
                pitch = Mathf.Lerp(pitch, maxPitch, lerpAmount);

            // Convert back to direction vector
            float pitchRad = pitch * Mathf.Deg2Rad;
            float yawRad = yaw * Mathf.Deg2Rad;

            Vector3 newDirection = new Vector3(
                Mathf.Sin(yawRad) * Mathf.Cos(pitchRad),
                Mathf.Sin(pitchRad),
                Mathf.Cos(yawRad) * Mathf.Cos(pitchRad)
            );

            return newDirection * magnitude;
        }

        private static Vector3 EnsureDownwardVelocity(Vector3 velocity, float maxPitch)
        {
            if (velocity == Vector3.zero) return velocity;

            float magnitude = velocity.magnitude;
            Vector3 direction = velocity.normalized;

            // Convert to spherical angles
            float pitch = Mathf.Asin(direction.y) * Mathf.Rad2Deg; // vertical angle
            float yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // Clamp pitch if needed
            if (pitch > maxPitch)
                pitch = maxPitch;

            // Convert back to direction vector
            float pitchRad = pitch * Mathf.Deg2Rad;
            float yawRad = yaw * Mathf.Deg2Rad;

            Vector3 newDirection = new Vector3(
                Mathf.Sin(yawRad) * Mathf.Cos(pitchRad),
                Mathf.Sin(pitchRad),
                Mathf.Cos(yawRad) * Mathf.Cos(pitchRad)
            );

            return newDirection * magnitude;
        }
    }


    [LandfallPlugin]
    public class SpeedDemonWalls
    {
        public static float rayHeightAdjustment = 400f; // Make sure no rays start clipping into ground
        public static float minimumMapHeight = -150f;
        public static float rayRadius = 250f;
        //public static float baseRayRadius = 150f;
        //public static float rayFallbackStepDistance = 100f;
        //public static int rayFallbackStepCount = 4;
        //public static float rayFallbackPercent = 0.7f;

        static SpeedDemonWalls()
        {
            Debug.Log("[SpeedDemon] SpeedDemonWalls class initializing...");
            On.GM_Run.Update += (orig, self) =>
            {
                RaycastHit[] rayHitMap = GetGroundRays(PlayerCharacter.localPlayer.transform.position, 64, 200f, 1500f);
                //Vector3? correctionVector = GetCorrectionVector(rayHitMap, 0.48f);
                Vector3? correctionVector = GetCorrectionVectorWithFallback(rayHitMap, 0.48f);
                //Vector3? correctionVector = GetCorrectionVectorNew();
                if (correctionVector != null)
                {
                    //correctionTimer = 0f;
                    //Vector3 newVelocity = CorrectHorizontalVelocity(PlayerCharacter.localPlayer.refs.rig.velocity, (Vector3)correctionVector, 0.25f);
                    Vector3 newVelocity = ClampYawWithCorrectionVector(PlayerCharacter.localPlayer.refs.rig.velocity, (Vector3)correctionVector, 88f);
                    PlayerCharacter.localPlayer.refs.rig.velocity = newVelocity;
                    //Debug.Log($"[SpeedDemon] Player hit a wall, setting velocity vector to {newVelocity}");
                    //Debug.Log($"[SpeedDemon] correctionVector is {correctionVector.Value}");
                }
                orig(self);
            };
        }

        public static bool rayHitGround(RaycastHit hit)
        {
            return hit.collider != null && hit.point.y >= minimumMapHeight;
        }

        private static Vector3? GetCorrectionVectorWithFallback(RaycastHit[] rayHitMap, float minNullChainPercent)
        {
            RaycastHit[] peripheralRaysDoubled = new RaycastHit[rayHitMap.Length * 2];
            Array.Copy(rayHitMap, 0, peripheralRaysDoubled, 0, rayHitMap.Length); // Copy the array twice so it can loop back on itself
            Array.Copy(rayHitMap, 0, peripheralRaysDoubled, rayHitMap.Length, rayHitMap.Length);
            // Find map direction
            int maxNullChain = 0;
            int currentNullChain = 0;
            int maxChainStartIndex = 0;
            int currentChainStartIndex = 0;
            // Loop through array and find the longest chain of nulls (longest segment of the circle that is empty) and the start index for it
            for (int i = 0; i < peripheralRaysDoubled.Length; i++)
            {
                RaycastHit hit = peripheralRaysDoubled[i];
                if (!rayHitGround(hit))
                {
                    currentNullChain++;
                    if (currentNullChain > maxNullChain)
                    {
                        maxNullChain = currentNullChain;
                        maxChainStartIndex = currentChainStartIndex;
                    }
                }
                else
                {
                    currentNullChain = 0;
                    currentChainStartIndex = i + 1;
                }
            }
            // Fewer than the specified rays were null in a row, likely above the map
            if (maxNullChain / (float)rayHitMap.Length < minNullChainPercent) return null; // Mod lets us ignore all misses case
            if (maxNullChain / (float)rayHitMap.Length > 0.7f)
            {
                return GetCorrectionVector(GetGroundRays(PlayerCharacter.localPlayer.transform.position, 64, 400f, 1500f), 0.48f);
            }
            // The player is likely over void, now we need a vector perpendicular to the map (pointing at it) to correct with
            // Recover horizontal radians from ray index
            float radians = ((maxChainStartIndex + maxNullChain / 2f) % rayHitMap.Length) / rayHitMap.Length * Mathf.PI * 2f;
            //Debug.Log($"[SpeedDemon] radians {radians}");
            //Debug.Log($"[SpeedDemon] maxChainStartIndex {maxChainStartIndex}");
            //Debug.Log($"[SpeedDemon] maxNullChain {maxNullChain}");
            //Debug.Log($"[SpeedDemon] rayHitMap.Length {rayHitMap.Length}");
            Vector3 negativeCorrectionVector = new Vector3(Mathf.Cos(radians), 0f, Mathf.Sin(radians));
            //Debug.Log($"[SpeedDemon] negativeCorrectionVector {negativeCorrectionVector}");
            // This vector points towards the void, we want to point towards the map, so negate
            return -negativeCorrectionVector;
        }

        private static RaycastHit[] GetGroundRays(Vector3 position, int peripheralRayCount, float checkRadius, float raycastDistance)
        {
            Vector3 adjustedPosition = new Vector3(position.x, position.y + rayHeightAdjustment, position.z); // Make sure no rays start clipping into ground
            RaycastHit[] hits = new RaycastHit[peripheralRayCount];

            // Peripheral rays
            for (int i = 0; i < peripheralRayCount; i++)
            {
                float angle = i * Mathf.PI * 2f / peripheralRayCount;
                Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * checkRadius;
                Vector3 rayOrigin = adjustedPosition + offset;

                Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastDistance);
                hits[i] = hit;
            }
            return hits;
        }

        private static Vector3? GetCorrectionVector(RaycastHit[] rayHitMap, float minNullChainPercent)
        {
            RaycastHit[] peripheralRaysDoubled = new RaycastHit[rayHitMap.Length * 2];
            Array.Copy(rayHitMap, 0, peripheralRaysDoubled, 0, rayHitMap.Length); // Copy the array twice so it can loop back on itself
            Array.Copy(rayHitMap, 0, peripheralRaysDoubled, rayHitMap.Length, rayHitMap.Length);
            // Find map direction
            int maxNullChain = 0;
            int currentNullChain = 0;
            int maxChainStartIndex = 0;
            int currentChainStartIndex = 0;
            // Loop through array and find the longest chain of nulls (longest segment of the circle that is empty) and the start index for it
            for (int i = 0; i < peripheralRaysDoubled.Length; i++)
            {
                RaycastHit hit = peripheralRaysDoubled[i];
                if (!rayHitGround(hit))
                {
                    currentNullChain++;
                    if (currentNullChain > maxNullChain)
                    {
                        maxNullChain = currentNullChain;
                        maxChainStartIndex = currentChainStartIndex;
                    }
                }
                else
                {
                    currentNullChain = 0;
                    currentChainStartIndex = i + 1;
                }
            }
            // Fewer than the specified rays were null in a row, likely above the map
            if ((maxNullChain % rayHitMap.Length) / (float)rayHitMap.Length < minNullChainPercent) return null; // Mod lets us ignore all misses case
            // The player is likely over void, now we need a vector perpendicular to the map (pointing at it) to correct with
            // Recover horizontal radians from ray index
            float radians = ((maxChainStartIndex + maxNullChain / 2f) % rayHitMap.Length) / rayHitMap.Length * Mathf.PI * 2f;
            //Debug.Log($"[SpeedDemon] radians {radians}");
            //Debug.Log($"[SpeedDemon] maxChainStartIndex {maxChainStartIndex}");
            //Debug.Log($"[SpeedDemon] maxNullChain {maxNullChain}");
            //Debug.Log($"[SpeedDemon] rayHitMap.Length {rayHitMap.Length}");
            Vector3 negativeCorrectionVector = new Vector3(Mathf.Cos(radians), 0f, Mathf.Sin(radians));
            //Debug.Log($"[SpeedDemon] negativeCorrectionVector {negativeCorrectionVector}");
            // This vector points towards the void, we want to point towards the map, so negate
            return -negativeCorrectionVector;
        }

        private static Vector3 ClampYawWithCorrectionVector(Vector3 velocity, Vector3 correctionVector, float maxDegrees)
        {
            // Project both vectors onto the XZ plane
            Vector3 velocityXZ = new Vector3(velocity.x, 0f, velocity.z);
            // Normalize them
            Vector3 velocityXZNorm = velocityXZ.normalized;
            Vector3 correctionVectorNorm = correctionVector.normalized;
            // Get angle between directions
            float angle = Vector3.Angle(correctionVectorNorm, velocityXZNorm);
            // If within limit, return original vector
            if (angle <= maxDegrees)
                return velocity;
            // Clamp angle by rotating the reference vector toward the original
            float t = maxDegrees / angle;
            Vector3 clampedDirection = Vector3.Slerp(correctionVectorNorm, velocityXZNorm, t);
            // Maintain original magnitude in XZ
            float magnitudeXZ = velocityXZ.magnitude;
            Vector3 finalXZ = clampedDirection * magnitudeXZ;
            // Restore original Y component
            return new Vector3(finalXZ.x, velocity.y, finalXZ.z);
        }

    }
}