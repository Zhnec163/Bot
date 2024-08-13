using UnityEngine;

public class BaseClickHandler : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private Base _selectedBase;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity) == false)
                return;

            if (hit.transform.TryGetComponent(out Base commandCenter))
                _selectedBase = commandCenter;

            if (_selectedBase == null)
                return;

            if (hit.transform.TryGetComponent(out Ground ground))
            {
                float offsetY = 0.5f;
                Vector3 position = new Vector3(hit.point.x, hit.point.y + offsetY, hit.point.z);
                
                if (_selectedBase.IsBuildBase)
                    _selectedBase.ChangeFlagPosition(position);
                else
                    _selectedBase.StartBuildingBase(position);
                    
                _selectedBase = null;
            }
        }
    }
}