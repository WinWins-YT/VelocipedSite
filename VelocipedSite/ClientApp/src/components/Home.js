import React, { Component } from 'react';
import {Link} from "react-router-dom";

export class Home extends Component {
  static displayName = Home.name;
  
  constructor(props) {
      super(props);
      this.state = {shops: [], loading: true};
  }
  
  componentDidMount() {
      document.title = "Главная";
      this.populateShopsList();
  }

    async populateShopsList() {
      const response = await fetch("/api/v1/Shops/GetShops", {
          method: "POST",
          headers: {
              "Content-Type": "application/json"
          },
          body: "{}"
      });
      const data = await response.json();
      this.setState({shops: data.shops, loading: false});
    }
    
    renderShops(shops) {
      return shops.map(x =>
          <div className={"col-md-3 d-flex align-items-center shopBannerContainer"}>
              <Link to={"/catalog/categories?shop=" + x.shopId}>
                  <img className={"shopBanner"} src={"images/" + x.pathToImg} alt={""}/>
              </Link>
          </div>
      );
    }

    render() {
      let content = this.state.loading
          ? <p><em>Loading...</em></p>
          : this.renderShops(this.state.shops);
          
        return (
            <div style={{justifyContent: "center"}} className={"row"}>
                <h3 style={{textAlign: "center"}}>Добро пожаловать в службу доставки товаров из супермаркетов "Велосипед"</h3>
                <p style={{textAlign: "center"}}>Выберите магазин из которого будет произведена доставка:</p>
                {content}
            </div>
        );
    }
}
