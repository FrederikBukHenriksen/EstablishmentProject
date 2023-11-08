import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import Chart, { ChartOptions } from 'chart.js/auto';
import {
  AnalysisClient,
  AuthenticationClient,
  LineChartData,
  LoginCommand,
} from 'api';

@Component({
  selector: 'app-create-establishment',
  templateUrl: './create-establishment.component.html',
  styleUrls: ['./create-establishment.component.scss'],
})
export class CreateEstablishmentComponent implements OnInit {
  private readonly authenticationClient = inject(AuthenticationClient);
  private readonly analysisClient = inject(AnalysisClient);
  applyForm = new FormGroup({
    firstName: new FormControl(''),
    lastName: new FormControl(''),
  });

  chart!: Chart;

  lolcat: boolean = true;

  public buttonColor = 'blue';

  public data!: LineChartData;

  ngOnInit(): void {
    this.analysisClient.productSalesChart().subscribe({
      next: (data) => {
        this.data = data;
      },
    });

    this.chart = this.createChart(this.data);
  }

  createChart(data: LineChartData): Chart {
    return new Chart('canvas', {
      data: {
        datasets: [
          {
            type: 'line',
            label: 'Line Dataset',
            // data: [20, 30, 40, 50],
            data: data.values.map((x) => x.item2),
          },
        ],
        labels: data.values.map((x) => x.item1),
        // labels: [
        //   '8:00',
        //   '9:00',
        //   '10:00',
        //   '11:00',
        //   '12:00',
        //   '13:00',
        //   '14:00',
        //   '15:00',
        //   '16:00',
        //   '17:00',
        //   '18:00',
        //   '19:00',
        //   '20:00',
        // ],
      },
      options: this.getOptions(),
    });
  }

  private getOptions(): ChartOptions {
    return {
      scales: {
        y: {
          beginAtZero: this.lolcat,
        },
      },
    };
  }

  private updateChart() {
    this.chart.options = this.getOptions();
    this.chart.update();
  }

  protected onSubmit() {
    this.lolcat = false;
    this.updateChart();
    this.authenticationClient
      .logIn({
        username: this.applyForm.value.firstName,
        password: this.applyForm.value.lastName,
      } as LoginCommand)
      .subscribe({
        next: (v) => {
          this.buttonColor = 'blue';
          console.log(v);
        },
        error: (e: HttpErrorResponse) => {
          this.buttonColor = 'red';
          console.error('fejlowitz', e.status);
        },
        complete: () =>
          this.authenticationClient
            .getLoggedInUser()
            .subscribe((user) => console.log('brugerinfo', user)),
      });
  }
}
