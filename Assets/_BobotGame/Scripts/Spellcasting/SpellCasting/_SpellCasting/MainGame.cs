using UnityEngine;

namespace SpellCasting
{
    public class MainGame : MonoBehaviour
    {


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

            SavedData = SavedData.LoadOrCreate();
        }

        private void Update()
        {
            EscapeInput.UpdateInput(Input.GetKey(KeyCode.Escape));

            TabInput.UpdateInput(Input.GetKey(KeyCode.Tab));
        }
    }
}