import React, { Component } from 'react';

export class Counter extends Component {
  static displayName = Counter.name;

  constructor(props) {
    super(props);
    this.state = { currentCount: 0, shopName: "", loading: true };
    this.incrementCounter = this.incrementCounter.bind(this);
  }

  incrementCounter() {
    this.setState({
      currentCount: this.state.currentCount + 1
    });
  }
  
  componentDidMount() {
      this.getShopName(new URL(window.location.href).searchParams.get("shop"));
  }

  async getShopName(shop) {
      const response = await fetch("/api/v1/Shops/GetShopById?id=" + shop);
      const data = await response.json();
      this.setState({shopName: data.shop.name, loading: false});
  }

  render() {
    return (
      <div>
          <h1>Counter</h1>

          {this.state.loading 
              ? "Loading..." 
              : <p>Selected shop: {this.state.shopName}</p>}

          <p>This is a simple example of a React component.</p>

           <p aria-live="polite">Current count: <strong>{this.state.currentCount}</strong></p>

           <button className="btn btn-primary" onClick={this.incrementCounter}>Increment</button>
      </div>
    );
  }
}
