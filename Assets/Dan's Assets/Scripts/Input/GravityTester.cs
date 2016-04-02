using UnityEngine;
using UnityEngine.UI;

namespace Rolling {

    public class GravityTester : MonoBehaviour {
        // HIDDEN FIELDS
        private LineRenderer _line;
        private float _lineWidth = 0.1f;

        // INSPECTOR FIELDS
        public Texture2D CursorImg;
        public Text InfoTxt;
        
        // EVENT HANDLERS
        private void Awake() {
            // Set cursor
            Cursor.SetCursor(CursorImg, new Vector2(16f, 16f), CursorMode.Auto);

            // Set up the gravity line renderer
            _line = gameObject.AddComponent<LineRenderer>();
            _line.SetWidth(_lineWidth, _lineWidth);
            _line.SetColors(Color.yellow, Color.red);
            _line.material = new Material(Shader.Find("Particles/Additive"));
        }
        private void Update() {
            // Get player input
            bool mouseDown = Input.GetMouseButton(0);

            // Render the gravity line
            Vector2 centerPixels = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Vector2 centerPt = Camera.main.ScreenToWorldPoint(centerPixels);
            Vector2 screenPt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _line.SetPosition(0, centerPt);
            _line.SetPosition(1, screenPt);

            // If the mouse is being held down, then adjust gravity
            if (mouseDown) {
                Vector2 newGravity = screenPt - centerPt;
                Physics2D.gravity = newGravity;
                InfoTxt.text = string.Format("Gravity:  <{0}, {1}>", newGravity.x, newGravity.y);
            }
        }
    }

}
