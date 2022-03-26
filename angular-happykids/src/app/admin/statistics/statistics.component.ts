import { Component, OnInit } from '@angular/core';
import { Statistics } from 'src/app/shared/models/statistics';
import { StatisticsService } from './statistics.service';
import {
  ChartErrorEvent,
  ChartMouseLeaveEvent,
  ChartMouseOverEvent,
  ChartSelectionChangedEvent,
  ChartType,
  Column,
  GoogleChartComponent
} from 'angular-google-charts';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.scss']
})
export class StatisticsComponent implements OnInit {
  statistics: Statistics;

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
    this.loadStatistics();
    this.getNumberOfBuyers();
  }

  loadStatistics() {
    return this.statisticsService.getCountForEntities()
    .subscribe(response => {
    this.statistics = response;
    }, error => {
    console.log(error);
    });
  }

  getNumberOfBuyers() {
    this.statisticsService.getNumberOfBuyersForEachPaymentOption().subscribe(
      result => {
        this.data = [];
        this.title = 'Payment Options for Children Items Consumed';
        this.type = ChartType.PieChart;
        console.log(result.list);
        for (const data in result.list) {
          if (data) {
            this.data.push([result.list[data].paymentOption.toString(),
              result.list[data].numberOfBuyers]);
          }
        }
      },
      error => {
        console.log(error);
      }
    );
  }


}




