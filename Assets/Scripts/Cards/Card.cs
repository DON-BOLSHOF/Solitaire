using UnityEngine;
using UnityEngine.UI;

namespace Cards
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private Image _frontGround;
        
        public void DynamicInstantiate(Sprite cardBack)
        {
            _frontGround.sprite = cardBack;
        }

        public void ActivateMainView(bool value)
        {
            _frontGround.gameObject.SetActive(value);
        }
    }
}