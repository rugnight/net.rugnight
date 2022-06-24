using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace rc.GameMode
{
    /// <summary>
    /// GameMode�֘A�̏����̃w���p�[
    /// </summary>
    public static class GameModeHelper
    {
        /// <summary>
        /// �v���t�@�u�Ȃ�Ε������ĕԂ��A�����łȂ���΂��̂܂܋A��
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static public GameObject InstanciateIfPrefab(GameObject obj)
        {
            if (obj.scene.IsValid())
            {
                return obj;
            }
            return GameObject.Instantiate<GameObject>(obj);
        }
    }

    public class GameMode : MonoBehaviour
    {
        // --------------------------------------------------
        // SerializeField
        // --------------------------------------------------
        [SerializeField]
        GameObject Player;

        [SerializeField]
        GameObject Pawn;

        [SerializeField]
        GameObject Hud;

        IPlayer m_Player;
        IPawn m_Pawn;
        IHud m_Hud;

        private void Awake()
        {
            m_Player = GameModeHelper.InstanciateIfPrefab(Player)?.GetComponent<IPlayer>();
            m_Pawn = GameModeHelper.InstanciateIfPrefab(Pawn)?.GetComponent<IPawn>();
            m_Hud = GameModeHelper.InstanciateIfPrefab(Hud)?.GetComponent<IHud>();

            m_Player?.Posses(m_Pawn);
            m_Player?.SetHud(m_Hud);
            m_Player?.SetCamera(m_Pawn?.GetCamera());
        }
    }
}
