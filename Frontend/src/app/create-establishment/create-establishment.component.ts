import { HttpErrorResponse } from '@angular/common/http';
import {
  Component,
  OnChanges,
  OnInit,
  SimpleChanges,
  inject,
} from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import Chart, { ChartOptions } from 'chart.js/auto';
import {
  AnalysisClient,
  AuthenticationClient,
  LoginCommand,
  GraphDTO,
  EstablishmentClient,
  Item,
  TimeAndValue,
  GetProductSalesPerDayQuery,
  TimeResolution,
} from 'api';
import { DateToTime } from '../utils/helper';
import { lastValueFrom, switchMap } from 'rxjs';

@Component({
  selector: 'app-create-establishment',
  templateUrl: './create-establishment.component.html',
  styleUrls: ['./create-establishment.component.scss'],
})
export class CreateEstablishmentComponent implements OnInit {
  private readonly authenticationClient = inject(AuthenticationClient);
  private readonly analysisClient = inject(AnalysisClient);
  private readonly establishmentClient = inject(EstablishmentClient);

  applyForm = new FormGroup({
    firstName: new FormControl(''),
    lastName: new FormControl(''),
  });

  private items: Item[] = [];
  private itemsSold?: [Item, TimeAndValue[]?];

  chart!: Chart;

  public data!: GraphDTO;

  ngOnInit(): void {
    this.establishmentClient.itemGetAll().subscribe({
      next: (x) => {
        this.items = x;
      },
    });

    var itemsSold = this.items.map((item) => [item, undefined]);
    this.getGraph();
  }

  private getGraph() {
    var command: GetProductSalesPerDayQuery = {
      itemId: '00000000-0000-0000-0000-000000000001',
      resolution: TimeResolution.Hour,
      startDate: new Date(new Date().setUTCHours(9, 0, 0, 0)),
      endDate: new Date(new Date().setUTCHours(15, 0, 0, 0)),
    };

    this.analysisClient.productSalesChart(command).subscribe({
      next: (data) => {
        this.data = data;
        this.chart = this.createChart(this.data);
      },
    });
  }

  createChart(data: GraphDTO): Chart {
    return new Chart('canvas', {
      data: {
        datasets: [
          {
            type: 'line',
            data: data.values!.map((x) => x.salesCount),
          },
        ],
        labels: data.values!.map((x) => DateToTime(x.date)),
      },
      options: this.getOptions(),
    });
  }
  private updateChart() {
    this.chart.update();
  }
}
