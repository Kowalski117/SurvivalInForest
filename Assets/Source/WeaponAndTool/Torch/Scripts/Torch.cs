using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] private PlayerEquipmentHandler _playerEquipmentHandler;
    [SerializeField] private PlayerAnimatorHandler _playerAnimatorHandler;
    [SerializeField] private AudioClip _gorenjeClip;
    [SerializeField] private AudioClip _hitClip;

    private AudioSource _audioSource;
    private ToolItemData _currentTool;
    private bool _isEnable = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _playerEquipmentHandler.OnUpdateToolItemData += Init;
    }

    private void OnDisable()
    {
        _playerEquipmentHandler.OnUpdateToolItemData -= Init;
    }

    private void Update()
    {
        if (_isEnable && _currentTool != null)
            _playerEquipmentHandler.UpdateDurabilityWithGameTime(_playerEquipmentHandler.CurrentInventorySlot);
    }

    private void Init(ToolItemData itemData)
    {
        if (itemData != null && itemData.ToolType == ToolType.Torch)
        {
            _currentTool = itemData;
            _isEnable = true;

            if (!_audioSource.isPlaying)
            {
                _audioSource.clip = _gorenjeClip;
                _audioSource.Play();
            }
        }
        else
        {
            _currentTool = null;
            _isEnable = false;
            _audioSource.Stop();
        }
    }
}
