using UnityEngine;
using UnityEngine.AI;

public class Barrack : Building {
    [Space]
    [SerializeField] private GameObject _canvas;
    [SerializeField] private Transform _spawn;

    public void CreateUnit(Unit unit) {
        Vector3 pos = _spawn.position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        
        Unit agent = Instantiate(unit, _spawn.position, _spawn.rotation).GetComponent<Unit>();

        agent.WhenClickOnGround(pos);
    }

    public override void Select() {
        base.Select();

        _canvas.SetActive(true);
    }

    public override void Unselect() {
        base.Unselect();

        _canvas.SetActive(false);
    }
}
