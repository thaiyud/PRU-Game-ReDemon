using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BossDarkController : MonoBehaviour
{
	[SerializeField] private SlashSkill slashSkill;
	[SerializeField] private PlasmaBurstSkill plasmaBurstSkill;
	[SerializeField] private ElectricBallSpawner electricBallSpawner;
	[SerializeField] private FlameSkill flameSkill;
	[SerializeField] private float GapBetweenSkill = 2f;

	private void Start()
	{
		StartCoroutine(ManageActivation());
	}

	private IEnumerator ManageActivation()
	{
		yield return new WaitForSeconds(GapBetweenSkill);

		while (true)
		{

			if (!slashSkill.gameObject.activeInHierarchy)
			{
				slashSkill.gameObject.SetActive(true);
			}

			slashSkill.Run();

			yield return new WaitUntil(() => slashSkill.IsActingComplete);

			slashSkill.gameObject.SetActive(false);

			yield return new WaitForSeconds(GapBetweenSkill);

			if (!plasmaBurstSkill.gameObject.activeInHierarchy)
			{
				plasmaBurstSkill.gameObject.SetActive(true);
			}

			plasmaBurstSkill.Run();
			yield return new WaitUntil(() => plasmaBurstSkill.IsActingComplete);

			plasmaBurstSkill.gameObject.SetActive(false);

			yield return new WaitForSeconds(GapBetweenSkill);

			if (!electricBallSpawner.gameObject.activeInHierarchy)
			{
				electricBallSpawner.gameObject.SetActive(true);
			}
			electricBallSpawner.Run();

			yield return new WaitUntil(() => electricBallSpawner.IsActingComplete);
			electricBallSpawner.gameObject.SetActive(false);

			yield return new WaitForSeconds(GapBetweenSkill);


			if (!flameSkill.gameObject.activeInHierarchy)
			{
				flameSkill.gameObject.SetActive(true);
			}

			flameSkill.Run();
			yield return new WaitUntil(() => flameSkill.IsActingComplete);
			flameSkill.gameObject.SetActive(false);

			yield return new WaitForSeconds(GapBetweenSkill);
		}
	}
}
