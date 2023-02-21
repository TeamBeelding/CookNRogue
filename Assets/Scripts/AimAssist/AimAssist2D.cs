using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AimAssist2D
{
    public static Vector3 CorrectAimDirection(Vector3 baseDirection, Vector3 origin, GameObject[] targets, AimAssistPreset preset)
    {
        #region Get Valid Target
        //Get all potential valid targets
        List<GameObject> tempValidTargets = new();
        foreach (GameObject target in targets)
        {
            //Check distance
            if (Vector3.Distance(origin, target.transform.position) > preset.GetMaxDistance)
            {
                continue;
            }

            //Check angle
            Vector3 tempTargetDir = (target.transform.position - origin).normalized;
            float angle = Vector3.SignedAngle(tempTargetDir, baseDirection, Vector3.up);
            if ((angle < 0 && angle < -preset.GetMaxAngle) || (angle > 0 && angle > preset.GetMaxAngle))
            {
                continue;
            }

            //Check line of sight
            if (!Physics.Raycast(origin, tempTargetDir, out RaycastHit hit, preset.GetMaxDistance) || hit.collider.gameObject != target)
            {
                continue;
            }

            Debug.Log(target);
            tempValidTargets.Add(target);
        }

        //If no valid target, return base aim direction
        if (tempValidTargets.Count == 0)
        {
            return baseDirection;
        }

        //Get valid target whose angle to the player aim direction is the smallest
        GameObject validTarget = null;
        float smallestAngle = Mathf.Infinity;
        for (int i = 0; i < tempValidTargets.Count; i++)
        {
            //Get angle
            Vector3 tempTargetDir = (tempValidTargets[i].transform.position - origin).normalized;
            float angle = Vector3.SignedAngle(tempTargetDir, baseDirection, Vector3.up);

            //Check
            if (Mathf.Abs(angle) < smallestAngle)
            {
                smallestAngle = angle;
                validTarget = tempValidTargets[i];
            }
        }
        #endregion

        Debug.Log(validTarget);

        #region Correct direction
        Vector3 targetDir = (validTarget.transform.position - origin).normalized;
        float curAngle = Vector3.SignedAngle(targetDir, baseDirection, Vector3.up);
        //Get current progression to the target direction
        float curStep = 1 - Mathf.Abs(curAngle) / preset.GetMaxAngle;
        //Correct progression based on the animationCurve
        float correctedStep = preset.GetAssistOffsetCurve.Evaluate(curStep);
        //Get angle extremity to progress from
        Vector3 angleExtremity = Quaternion.AngleAxis(Mathf.Sign(curAngle) * preset.GetMaxAngle, Vector3.up) * targetDir;

        //Correct aim direction
        Vector3 correctedDir = Vector3.Lerp(angleExtremity, targetDir, correctedStep);
        return correctedDir;
        #endregion
    }
}

