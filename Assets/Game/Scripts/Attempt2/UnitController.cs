using UnityEngine;

namespace Prototype
{
    public class UnitController : MonoBehaviour
    {
        [SerializeField] Unit SelectedUnit;

        Navigation.Node previousNodeUnderCursor;
        Unit previousUnitUnderCursor;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(SelectedUnit)
                {
                    SelectedUnit.DisableLine();
                }
                SelectedUnit = Utility.ObjectOfTypeUnderCursor<Unit>();
            }

            if (SelectedUnit && !SelectedUnit.Agent.IsMoving)
            {
                Navigation.Node nodeUnderCursor = Utility.ObjectOfTypeUnderCursor<Navigation.Node>();

                if (nodeUnderCursor && nodeUnderCursor.IsTraversible)
                {
                    if (nodeUnderCursor != previousNodeUnderCursor && SelectedUnit)
                    {
                        SelectedUnit.Agent.UpdatePathTo(nodeUnderCursor);
                        SelectedUnit.SetPathPoints();
                    }
    
                    if (SelectedUnit && Input.GetMouseButtonDown(1))
                    {
                        SelectedUnit.Agent.Move();
                        nodeUnderCursor = null;
                    }
                    previousNodeUnderCursor = nodeUnderCursor;
                }

                Unit unitUnderCursor = Utility.ObjectOfTypeUnderCursor<Unit>();

                if (unitUnderCursor && unitUnderCursor != previousUnitUnderCursor && SelectedUnit)
                {
                    SelectedUnit.TargetUnit(unitUnderCursor);
                }

                previousUnitUnderCursor = unitUnderCursor;
            }
        }
    }
}