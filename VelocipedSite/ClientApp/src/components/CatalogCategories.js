import React, {useEffect, useState} from "react";
import {Link} from "react-router-dom";

export default function CatalogCategories() {
    
    const shopId = new URL(window.location.href).searchParams.get("shop");
    
    const [shopImgUrl, setShopImgUrl] = useState("");
    const [catalog, setCatalog] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        document.title = "Категории";
        getShop(shopId);
        getShopCatalog(shopId);

        // eslint-disable-next-line
    }, [shopId]);
    
    async function getShop(shop) {
        const response = await fetch("/api/v1/Shops/GetShopById", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                id: shopId
            })
        });
        const data = await response.json();
        setShopImgUrl(data.shop.pathToImg);
    }
    
    async function getShopCatalog(shop) {
        const response = await fetch("/api/v1/Catalog/GetCatalogCategories", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                shopId: shopId
            })
        });
        const data = await response.json();
        setCatalog(data.categories);
        setLoading(false);
    }
    
    function renderCatalog(catalog) {
        return catalog.map(x =>
            <div className={"col-md-4 d-flex align-items-center"}>
                <Link className={"categoryBanner"} to={"/category?shop=" + shopId + "&id=" + x.id}>
                    <div>
                        <img className={"align-items-center align-self-center justify-center"} src={"images/products/" + shopId + "/" + x.pathToImg} alt={""}/>
                        <p>{x.name}</p>
                    </div>
                </Link>
            </div>
        );
    }

    return (
        <>
            <img className={"shopBanner d-flex align-items-center"} src={"images/" + shopImgUrl} alt={""}/><br/>
            <div style={{justifyContent: "center"}} className={"row"}>
                <h3>Категории:</h3>
                {loading
                    ? <p style={{textAlign: "center"}}><em>Loading...</em></p>
                    : renderCatalog(catalog)}
            </div>
        </>
    )
}