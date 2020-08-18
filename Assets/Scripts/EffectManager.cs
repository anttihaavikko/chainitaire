﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectManager : MonoBehaviour {

	public AutoEnd[] effects;
    public DamageNumber numberPrefab;

    [SerializeField]
    private Queue<AutoEnd>[] effectPool;

    [SerializeField]
    private TextEffectPool textPool;

    // ==================

    private static EffectManager instance = null;

	public static EffectManager Instance {
		get { return instance; }
	}

	// ==================

	void Awake() {
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
			return;
		} else {
			instance = this;
		}

        effectPool = new Queue<AutoEnd>[effects.Length];

        for(var i = 0; i < effectPool.Length; i++)
        {
            effectPool[i] = new Queue<AutoEnd>();
        }
    }

	public GameObject AddEffect(int effect, Vector3 position, float angle = 0f) {
		var e = Get(effect);
        e.transform.parent = transform;
		e.transform.position = position;
        e.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
		return e.gameObject;
	}

	public GameObject AddEffectToParent(int effect, Vector3 position, Transform parent) {
        var e = Get(effect);
        e.transform.parent = parent;
		e.transform.position = position;
		return e.gameObject;
	}

    public void AddText(string message, Vector3 position, float scale = 1f)
    {
        var t = textPool.Get();
        t.SetNumber(message, scale);
        t.transform.position = position;
        t.QueueRemove();
    }

    private AutoEnd Get(int index)
    {
        if (!effectPool[index].Any())
            AddObjects(index, 1);

        var obj = effectPool[index].Dequeue();
        obj.gameObject.SetActive(true);
        obj.Start();
        obj.GetParticleSystem().Play();

        return obj;
    }

    private void AddObjects(int index, int count)
    {
        for (var i = 0; i < count; i++)
        {
            var obj = Instantiate(effects[index]);
            obj.Pool = index;
            effectPool[index].Enqueue(obj);
        }
    }

    public void ReturnToPool(AutoEnd obj)
    {
        obj.gameObject.SetActive(false);
        effectPool[obj.Pool].Enqueue(obj);
    }

    public void ReturnTextToPool(DamageNumber obj) {
        textPool.ReturnToPool(obj);
    }
}