import React, { Component } from 'react';
import {Link} from "react-router-dom";

export class Home extends Component {
  static displayName = Home.name;

  render() {
        return (
            <div style={{justifyContent: "center"}} className={"row"}>
                <div className={"col-md-4 d-flex align-items-center shopBannerContainer"}>
                    <Link to={"/counter?shop=shesterochka"}>
                        <img className={"shopBanner"} src={"images/shet.jpg"} alt={""}/>
                    </Link>
                </div>
                <div className={"col-md-4 d-flex align-items-center shopBannerContainer"}>
                    <Link to={"/counter?shop=waypma"}><
                        img className={"shopBanner"} src={"images/waypma.jpeg"} alt={""}/>
                    </Link>
                </div>
                <div className={"col-md-4 d-flex align-items-center shopBannerContainer"}>
                    <Link to={"/counter?shop=fox"}>
                        <img className={"shopBanner"} src={"images/fox.png"} alt={""}/>
                    </Link>
                </div>
                <div className={"col-md-4 d-flex align-items-center shopBannerContainer"}>
                    <Link to={"/counter?shop=fox"}>
                        <img className={"shopBanner"} src={"images/fox.png"} alt={""}/>
                    </Link>
                </div>
                <div className={"col-md-4 d-flex align-items-center shopBannerContainer"}>
                    <Link to={"/counter?shop=fox"}>
                        <img className={"shopBanner"} src={"images/fox.png"} alt={""}/>
                    </Link>
                </div>
            </div>
        );
  }
}
