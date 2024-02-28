import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
  inject,
} from '@angular/core';
import {
  Chart,
  ChartData,
  ChartDataset,
  ChartOptions,
  ChartType,
} from 'chart.js';

@Component({
  selector: 'app-graph',
  templateUrl: './graph.component.html',
  styleUrls: ['./graph.component.scss'],
})
export class GraphComponent implements OnInit, OnChanges {
  @Input() chartType: ChartType = 'scatter';
  @Input() chartData: ChartData = {
    labels: [],
    datasets: [],
  } as ChartData;
  @Input() chartOptions: ChartOptions = {
    legend: {
      display: false,
    },
  } as ChartOptions;

  chart: Chart = new Chart('canvas', {
    type: this.chartType,
    data: {
      labels: [],
      datasets: [],
    },
    options: {
      legend: {
        display: false,
      },
    } as ChartOptions,
  });
  type!: ChartType;
  data!: ChartData;
  options!: ChartOptions;

  ngOnInit(): void {
    this.chart = this.CreateChart();
  }

  ngOnChanges(changes: SimpleChanges): void {
    console.log(changes);
    if (changes['chartData']) {
      console.log(changes['chartData'].currentValue);

      this.data = changes['chartData'].currentValue;
    }
    if (changes['chartOptions']) {
      this.options = changes['chartOptions'].currentValue;
    }

    if (changes['ChartType']) {
      this.type = changes['ChartType'].currentValue;
    }
    this.UpdateChart();
    console.log('data', this.data);
  }

  private CreateChart(): Chart {
    const chart = new Chart('canvas', {
      type: 'line',
      data: this.data,
      options: this.options,
    });
    return chart;
  }

  private UpdateChart(): void {
    this.chart.data = this.data;
    this.chart.update();
  }
}
