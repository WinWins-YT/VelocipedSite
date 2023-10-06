import React, {useEffect, useState} from "react";
import {Link} from "react-router-dom";

export default function CatalogCategories() {
    
    const shopId = new URL(window.location.href).searchParams.get("shop");
    
    const [shopImgUrl, setShopImgUrl] = useState("");
    const [catalog, setCatalog] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        getShop(shopId);
        getShopCatalog(shopId);
    }, [shopId]);
    
    async function getShop(shop) {
        const response = await fetch("/api/v1/Shops/GetShopById?id=" + shop);
        const data = await response.json();
        setShopImgUrl(data.shop.pathToImg);
    }
    
    async function getShopCatalog(shop) {
        const response = await fetch("/api/v1/Catalog/GetCatalogCategories?shopId=" + shop);
        const data = await response.json();
        setCatalog(data.categories);
        setLoading(false);
    }
    
    function renderCatalog(catalog) {
        console.log(catalog);
        return catalog.map(x =>
            <div className={"col-md-4 d-flex align-items-center"}>
                <Link to={"/category?shop=" + shopId + "&id=" + x.id}>
                    <img className={"shopBanner"} src={"images/products/" + shopId + "/" + x.pathToImg} alt={""}/>
                </Link>
            </div>
        );
    }

    return (
        <>
            <img className={"shopBanner d-flex align-items-center"} src={"images/" + shopImgUrl} alt={""}/><br/>
            <div style={{justifyContent: "center"}} className={"row"}>
                <h3>Популярное:</h3>
                {loading
                    ? <p style={{textAlign: "center"}}><em>Loading...</em></p>
                    : renderCatalog(catalog)}
            </div>
        </>
    )
}