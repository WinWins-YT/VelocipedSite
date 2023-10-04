import React, { Component } from 'react';

export class Counter extends Component {
  static displayName = Counter.name;

  constructor(props) {
    super(props);
    this.state = { currentCount: 0 };
    this.incrementCounter = this.incrementCounter.bind(this);
  }

  incrementCounter() {
    this.setState({
      currentCount: this.state.currentCount + 1
    });
  }
  
  getShopName(shop) {
      switch (shop) {
          case "shesterochka":
              return "Шестерочка";
              
          case "waypma":
              return "Ларек с шаурмой";
              
          case "fox":
              return "Лисья дыра";
              
          default:
              return "Неизвестный"
      }
  }

  render() {
    return (
      <div>
          <h1>Counter</h1>

          <p>Selected shop: {this.getShopName(new URL(window.location.href).searchParams.get("shop"))}</p>

          <p>This is a simple example of a React component.</p>

           <p aria-live="polite">Current count: <strong>{this.state.currentCount}</strong></p>

           <button className="btn btn-primary" onClick={this.incrementCounter}>Increment</button>
      </div>
    );
  }
}
