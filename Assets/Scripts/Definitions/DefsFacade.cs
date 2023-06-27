using UnityEngine;

namespace Definitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private CardBackDefs cardBackDefs;

        public CardBackDefs CardBackDefs => cardBackDefs;

        private static DefsFacade _instance;
        
        public static DefsFacade I => _instance ? _instance : LoadDefs();

        private static DefsFacade LoadDefs()
        {
            return _instance = Resources.Load<DefsFacade>("Definitions/DefsFacade");
        }
    }
}