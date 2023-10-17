import React, {useEffect, useState} from "react";

export default function Catalog() 
{
    const shopId = new URL(window.location.href).searchParams.get("shop");
    const id = new URL(window.location.href).searchParams.get("id");

    const [shopName, setShopName] = useState("");
    const [shopImgUrl, setShopImgUrl] = useState("");
    const [catalogName, setCatalogName] = useState("");
    const [catalogImgUrl, setCatalogImgUrl] = useState("");
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        getShop(shopId);
        getCatalog(id);
        document.title = `${catalogName} - ${shopName}`;
    }, []);

    async function getShop(shop) {
        const response = await fetch("/api/v1/Shops/GetShopById?id=" + shop);
        const data = await response.json();
        setShopName(data.shop.name);
        setShopImgUrl(data.shop.pathToImg);
    }
    
    async function getCatalog(categoryId) {
        const response = await fetch("/api/v1/Catalog/GetCatalogCategoryById?catalogId=" + categoryId);
        const data = await response.json();
        setCatalogName(data.catalogCategory.name);
        setCatalogImgUrl(data.catalogCategory.pathToImg);
    }
    
    async function getProducts(categoryId) {
        I
    }
    
    function renderCatalog() {
        
    }
    
    return (
        <div>
            <div style={{justifyContent: "center"}} className={"row"}>
                <img className={"shopBanner col-md-4 d-flex align-items-center"} src={"images/" + shopImgUrl} alt={""}/><br/>
                <img className={"shopBanner col-md-4 d-flex align-items-center"} src={`images/${shopId}/${catalogImgUrl}`} alt={""}/><br/>
            </div>
            {
                loading ? <p><em>Loading...</em></p>
                    : renderCatalog()
            }
        </div>
    )
}