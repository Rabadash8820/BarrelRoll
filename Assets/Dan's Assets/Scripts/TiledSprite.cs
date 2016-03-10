using UnityEngine;

namespace Rolling {

    // @NOTE the attached sprite's position should be "top left" or the children will not align properly
    // Strech out the image as you need in the sprite render, then the following script will generates a
    // nice set of repeated sprites inside its bounds
    public class TiledSprite : MonoBehaviour {
        // INSPECTOR FIELDS
        public SpriteRenderer TilePrefab;

        void Awake() {
            // Cache some values needed for looping
            float scaleX = transform.localScale.x;
            float scaleY = transform.localScale.y;
            float boundsX = TilePrefab.sprite.bounds.size.x;
            float boundsY = TilePrefab.sprite.bounds.size.x;
            int countX = Mathf.RoundToInt(scaleX / boundsX);
            int countY = Mathf.RoundToInt(scaleY / boundsY);

            // Loop through and spit out repeated tiles
            Vector2 localScale = new Vector2(1f / scaleX, 1f / scaleY);
            Vector2 corner = transform.position - new Vector3(Mathf.FloorToInt(scaleX / 2f), Mathf.FloorToInt(scaleY / 2f));
            for (int x = 0; x < countX; x++) {
                for (int y = 0; y < countY; y++) {
                    SpriteRenderer tile = Instantiate(TilePrefab);
                    tile.name = string.Format("Tile_{0},{1}", x, y);
                    Transform trans = tile.transform;
                    trans.parent = transform;
                    trans.position = corner + new Vector2(x * boundsX, y * boundsY);
                    trans.localScale = localScale;
                }
            }
        }

    }

}
