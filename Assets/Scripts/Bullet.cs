using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Coroutine _timeoutCoroutine;

    public void StartTimeoutCoroutine(float bulletTimeout)
    {
        _timeoutCoroutine = StartCoroutine(DestroyBulletAfterTimeout(bulletTimeout));
    }
    
    public void StopTimeoutCoroutine()
    {
        if (_timeoutCoroutine != null)
            StopCoroutine(_timeoutCoroutine);
    }

    private IEnumerator DestroyBulletAfterTimeout(float bulletTimeout)
    {
        yield return new WaitForSeconds(bulletTimeout);
        Destroy(gameObject);
        _timeoutCoroutine = null;
    }
}
