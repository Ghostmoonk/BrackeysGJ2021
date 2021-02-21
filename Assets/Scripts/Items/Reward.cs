using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour {
    [SerializeField]
    ItemType item;

    [SerializeField]
    float rotationSpeed;

    private PlayerLead player;
    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLead>();
    }

    // Update is called once per frame
    void Update() {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            player.Inventory.AddItem(item);
            Destroy(gameObject);
        }
    }
}
