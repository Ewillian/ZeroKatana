﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
	[SerializeField] MenuController menuController;
	Animator animator;
	[SerializeField] int thisIndex;
	public bool disableOnce;

	void Start()
    {
		animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
		if (menuController.index == thisIndex)
		{
			animator.SetBool("is_selected", true);
			if (Input.GetAxis("Submit") == 1)
			{
				animator.SetBool("is_pressed", true);
			}
			else if (animator.GetBool("is_pressed"))
			{
				animator.SetBool("is_pressed", false);
			}
		}
		else
		{
			animator.SetBool("is_selected", false);
		}
	}

	public void PlaySound(AudioClip buttonSound)
	{
		if (!disableOnce)
		{
			menuController.audioSource.PlayOneShot(buttonSound);
		}
		else
		{
			disableOnce = false;
		}
	}
}
