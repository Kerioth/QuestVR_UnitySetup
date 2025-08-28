using UnityEngine;
using UnityEngine.Events;

public class MoleHole : MonoBehaviour
{
    [SerializeField]
    private HitMole mole;
    private float _moveSpeed = 1f;
    private float _startY;
    public bool IsUp => mole.isUp;
    
    [Header("Events")]
    [Space]
    public UnityEvent<MoleHole> OnUp;
    public UnityEvent<MoleHole> OnDown;
    public UnityEvent<MoleHole> OnHit;
    
    void Start()
    {
        _startY = mole.transform.localPosition.y;
        mole.OnHit.AddListener(MoleHit);
    }

    public void RemoveListeners()
    {
        OnUp.RemoveAllListeners();
        OnDown.RemoveAllListeners();
        OnHit.RemoveAllListeners();
    }

    private void MoleHit()
    {
        //print("Hole see mole hit");
        OnHit?.Invoke(this);
        MoleDown();
    }

    public async void MoleUp(float waitTime = 1f)
    {
        if (mole.isUp) return;
        mole.isUp = true;
        
        //print("MoleUp");
        Vector3 pos = mole.transform.localPosition;
        pos.y = 0;
        
        while (mole.transform.localPosition.y < 0)
        {
            if (!mole.isUp) return;
            mole.transform.localPosition = Vector3.MoveTowards(mole.transform.localPosition, pos, _moveSpeed * Time.fixedDeltaTime);
            await Awaitable.FixedUpdateAsync();
        }
        
        // mole.transform.localPosition = pos;
        OnUp?.Invoke(this);
        
        await Awaitable.WaitForSecondsAsync(waitTime);
        MoleDown();
    }

    private async void MoleDown()
    {
        if (!mole.isUp) return;
        mole.isUp = false;
        
        //print("MoleDown");
        
        Vector3 pos = mole.transform.localPosition;
        pos.y = _startY;
        
        while (mole.transform.localPosition.y > _startY)
        {
            mole.transform.localPosition = Vector3.MoveTowards(mole.transform.localPosition, pos, _moveSpeed * Time.fixedDeltaTime);
            await Awaitable.FixedUpdateAsync();
        }
        
        // mole.transform.localPosition = pos;
        OnDown?.Invoke(this);
    }
}
