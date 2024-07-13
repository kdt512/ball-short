using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPManager : Singleton<IAPManager>
{
    private IStoreController controller;
    private IExtensionProvider extensions;
    IGooglePlayStoreExtensions ggExtensions;
    IAppleExtensions appleExtensions;

    public void InRestore(bool succes, string mess)
    {
        
    }

    public void OnProductFetched(ProductCollection productCollection)
    {
    }

    public void OnPurchaseComplete(Product product)
    {
        string productID = product.definition.id;
        Debug.Log("Product: " + productID);
        switch (productID)
        {
            case Constans.IAP_NOADS:
                DataManager.IsNoAds = true;
                Debug.Log("Product buy done: " + productID);
                break;
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason purchaseFailureReason)
    {
        Debug.LogError("Product failed: " + product.definition.id + " | " + purchaseFailureReason);
    }

    public void OnPurchaseDetailedFailedEvent(Product product, PurchaseFailureDescription purchaseFailureDescription)
    {
    }
}