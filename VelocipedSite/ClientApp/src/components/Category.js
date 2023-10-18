import React, {useEffect, useState} from "react";
import {Link} from "react-router-dom";

export default function Category() 
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
        getShop(shopId)
            .then(() => getCatalog(id)
                .then(() => document.title = `${catalogName} - ${shopName}`));
        getProducts();
    }, [catalogName, id, shopId, shopName]);

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
    
    async function getProducts() {
        const response = await fetch(`/api/v1/Products/GetProductsInCategory?catalogId=${id}&shopId=${shopId}`);
        const data = await response.json();
        setProducts(data.products);
        setLoading(false);
    }
    
    function renderCatalog() {
        return products.map(x => 
            <div className={"align-items-center"}>
                <p>{catalogName}</p>
                <div className={"col-md-4 d-flex align-items-center"}>
                    <Link className={"categoryBanner"} to={"/product?shop=" + shopId + "&id=" + x.id}>
                        <div>
                            <img src={"images/products/" + shopId + "/" + x.pathToImg} alt={""}/>
                            <p>{x.name}</p>
                            <p>{x.price} руб.</p>
                        </div>
                    </Link>
                </div>
            </div>
        );
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