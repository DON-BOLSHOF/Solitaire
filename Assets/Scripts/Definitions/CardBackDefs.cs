using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Definitions
{
    [CreateAssetMenu(menuName = "Defs/CardBackGroundDefs", fileName = "CardBackGroundDefs")]
    public class CardBackDefs : ScriptableObject
    {
        [SerializeField] private List<Sprite> _sprites;
        
        public int SpritesCount => _sprites.Count;

        public Sprite Get(string name)
        {
            return name == null ? default : _sprites.FirstOrDefault(itemDef => itemDef.name == name);
        }

        public Sprite Get(int id)
        {
            return _sprites[id];
        }

        public List<Sprite> GetAllCardsDefs()
        {
            return new List<Sprite>(_sprites);
        }
    }
}