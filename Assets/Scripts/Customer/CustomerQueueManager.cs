using System;
using System.Collections;
using System.Collections.Generic;
using CompletedMask;
using NaughtyAttributes;
using StateMachine.States;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Customer
{
	public class CustomerQueueManager : MonoBehaviour
	{
		public static event Action<Customer> CustomerGreeted;

		[BoxGroup("Settings")]
		[SerializeField] private int CustomersToSpawn;
		[BoxGroup("Settings")]
		[SerializeField] private float DistanceBetweenCustomers;

		[BoxGroup("References")]
		[SerializeField] private Customer[] CustomerPrefabs;
		[BoxGroup("References")]
		[SerializeField] private Transform SpawnPoint;

		[BoxGroup("Readonly")]
		[ReadOnly]
		[SerializeField] private List<Customer> SpawnedCustomers = new();

		private Customer _currentCustomer;
		private Customer _previouslySpawnedCustomer;

		private void Awake()
		{
			SpawnCustomers();
		}

		private void OnEnable()
		{
			StateMachine.StateMachine.OnStateChanged += StateMachine_OnStateChanged;
		}

		private void OnDisable()
		{
			StateMachine.StateMachine.OnStateChanged -= StateMachine_OnStateChanged;
		}

		private void StateMachine_OnStateChanged(StateBase previousState, StateBase currentState)
		{
			switch (currentState)
			{
				case CustomerDialogueState:
					ShowAllCustomers();
					StartCoroutine(ServeNextCustomer());
					break;

				default:
					HideAllCustomers();
					break;
			}
		}

		public void SetCurrentCustomerMaskItem(CompletedMaskItem maskItem)
		{
			_currentCustomer.SetCurrentMaskItem(maskItem);
		}

		public Customer GetCurrentCustomer()
		{
			return _currentCustomer;
		}

		private void SpawnCustomers()
		{
			for (int i = 0; i < CustomersToSpawn; i++)
			{
				Customer newCustomerGo = Instantiate(CustomerPrefabs[Random.Range(0, CustomerPrefabs.Length)], SpawnPoint.position, Quaternion.identity, SpawnPoint);

				// Want to make sure we never spawn the same customer twice in a row.
				while (newCustomerGo == _previouslySpawnedCustomer && CustomerPrefabs.Length > 1)
				{
					Destroy(newCustomerGo.gameObject);
					newCustomerGo = Instantiate(CustomerPrefabs[Random.Range(0, CustomerPrefabs.Length)], SpawnPoint.position, Quaternion.identity, SpawnPoint);
				}

				newCustomerGo.transform.position = new Vector3(SpawnPoint.position.x + i * DistanceBetweenCustomers, SpawnPoint.position.y, SpawnPoint.position.z);
				newCustomerGo.HideSpeechBubble();
				SpawnedCustomers.Add(newCustomerGo);
			}
		}

		private void HideAllCustomers()
		{
			foreach (Customer customer in SpawnedCustomers)
			{
				customer.Hide();
			}
		}

		private void ShowAllCustomers()
		{
			foreach (Customer customer in SpawnedCustomers)
			{
				customer.Show();
			}
		}

		private IEnumerator ServeNextCustomer()
		{
			if (SpawnedCustomers.Count == 0)
				yield break;

			if (_currentCustomer)
			{
				_currentCustomer.Farewell();

				yield return new WaitForSeconds(3f);
				_currentCustomer.HideSpeechBubble();
				Destroy(_currentCustomer.gameObject);
				SpawnedCustomers.RemoveAt(0);
			}

			_currentCustomer = SpawnedCustomers[0];
			_currentCustomer.ShowSpeechBubble();
			_currentCustomer.Greet();
			CustomerGreeted?.Invoke(_currentCustomer);

			yield return null;
		}
	}
}
