using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TempPlayer : MonoBehaviour
{
    // IntChange : 반환형이 없고, int 매개변수를 1개 받는
    //             함수를 담아둘 수 있는 타입이다.
    // inspector에 보여 드롭할 수 있다
    public UnityEvent TempEvent;
    public event Action<int> OnHealthChange;
    public ObservableProperty<float> Exp = new(0);
    private int _health;
    public int Health
    {
        get => _health;
        private set
        {
            _health = value;
            OnHealthChange?.Invoke(_health);
        }
    }

    public Action<int, float, string> a;

    // 콜백, 제공받은 함수를 역으로 호출
    // 백엔드에서 데이터 불러왔을 때 성공/실패 경우에 따라 나눠서 호출
    private void TryLoadData(Action s, Action f)
    {
        bool success = false;
        if (success)
        {
            s.Invoke();
        }
        else
        {
            f.Invoke();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) TakeDamage(5);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Heal(10);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Exp.Value += 20.5f;
    }

    private void OnDestroy() => Exp.RemoveAllListeners();


    public void TakeDamage(int damage)
    {
        Debug.Log("데미지 받음");
        Health -= damage;
    }

    public void Heal(int heal)
    {
        Debug.Log("회복함");
        Health += heal;
    }
}
