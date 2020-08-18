using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DamageNumber : MonoBehaviour
{
    public List<TMPro.TMP_Text> nums;

    // Start is called before the first frame update
    void Start()
    {
        QueueRemove();
    }

    public void QueueRemove()
    {
        Invoke("Remove", 3f);
    }

    public void SetNumber(string message, float scale = 1f)
    {
        nums.ForEach(num => num.text = message);
        transform.localScale *= scale;
    }

    // Update is called once per frame
    void Remove()
    {
        EffectManager.Instance.ReturnTextToPool(this);
    }
}
