using UnityEngine;

// A collectible keycard.
public class KeycardCollectible : Collectible
{
    [SerializeField] private KeycardType _type;

    public override void TryCollect(Character collector)
    {
        if (!collector.TryGetComponent(out CharacterKeycards keycards))
            return;

        keycards.Add(_type);
        Consume();
    }
}
