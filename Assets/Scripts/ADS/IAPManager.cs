using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour
{
    private const string RemoveAd = "kraken.adventure.squid.puzzle.RemoveAd";

    public void OnPurchaseComplete(Product product)
    {
        if (product.definition.id == RemoveAd)
        {
            Singleton<AdSettings>.Instance.RemoveAds();
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {

    }
}
