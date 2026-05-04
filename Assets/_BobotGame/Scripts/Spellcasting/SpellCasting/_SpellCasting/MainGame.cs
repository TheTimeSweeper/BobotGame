using SpellCasting.World;
using UnityEngine;

namespace SpellCasting
{
    public class MainGame : MonoBehaviour
    {
        [SerializeField]
        private RoomCatalog TEMPRoomCatalog;

        [SerializeField]
        private ElementCatalog TEMPElementCatalog;

        public static MainGame Instance { get; private set; }

        public SavedData SavedData { get; private set; }

        public static InputState EscapeInput = new InputState();

        public static InputState TabInput = new InputState();

        void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);

            Instance = this;
            InitializeGame();
            Object.DontDestroyOnLoad(gameObject);
        }

        public void InitializeGame()
        {
            TEMPElementCatalog.InitWithMainGame();

            SavedData = SavedData.LoadOrCreate();

            TEMPRoomCatalog.InitWithMainGame();
        }

        private void Update()
        {
            EscapeInput.UpdateInput(Input.GetKey(KeyCode.Escape));

            TabInput.UpdateInput(Input.GetKey(KeyCode.Tab));
        }
    }
}