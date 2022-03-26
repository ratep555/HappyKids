import { Component, OnInit } from '@angular/core';
import {
  ChartErrorEvent,
  ChartMouseLeaveEvent,
  ChartMouseOverEvent,
  ChartSelectionChangedEvent,
  ChartType,
  Column,
  GoogleChartComponent
} from 'angular-google-charts';
import { StatisticsService } from '../statistics.service';

@Component({
  selector: 'app-childrenitem-order-chart',
  templateUrl: './childrenitem-order-chart.component.html',
  styleUrls: ['./childrenitem-order-chart.component.scss']
})
export class ChildrenitemOrderChartComponent implements OnInit {
  title = '';
  type = ChartType.PieChart;
  data = [];
  columnNames: Column[] = ['', ''];
  options = {
    width: 700,
    height: 500,
    hAxis: { title: '' },
  };
  width = 950;
  height = 300;

  constructor(private statisticsService: StatisticsService) { }

  ngOnInit(): void {
    this.getOrderStatusesForChildrenItems();
  }

  getOrderStatusesForChildrenItems() {
    this.statisticsService.getAllOrderStatusesForChildrenItems().subscribe(
      result => {
        this.data = [];
        this.title = 'Order Statuses for Children Item Orders';
        this.type = ChartType.PieChart;
        console.log(result.list);
        for (const data in result.list) {
          if (data) {
            this.data.push([result.list[data].orderStatus.toString(),
              result.list[data].count]);
          }
        }
      },
      error => {
        console.log(error);
      }
    );
}

}
