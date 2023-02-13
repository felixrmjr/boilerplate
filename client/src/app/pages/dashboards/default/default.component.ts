import { Component, OnInit, ViewChild } from "@angular/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { EventService } from "../../../core/services/event.service";

// import DatalabelsPlugin from 'chartjs-plugin-datalabels';
import { ChartConfiguration, ChartData, ChartEvent, ChartType } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';

@Component({
	selector: "app-default",
	templateUrl: "./default.component.html",
	styleUrls: ["./default.component.scss"],
})
export class DefaultComponent implements OnInit {

    pieChart: ChartType;

	@ViewChild("content") content;
    @ViewChild(BaseChartDirective) chart: BaseChartDirective | undefined;

	constructor(private modalService: NgbModal, private eventService: EventService) {}

    public pieChartOptions: ChartConfiguration['options'] = {
        responsive: true,
        plugins: {
            legend: {
                display: false
            },
        },
        layout: {
            padding: {
                left: 0,
                right: 0,
                top: 0,
                bottom: 0,
            },
        },
    };

    public pieChartData: ChartData<'pie', number[], string | string[]> = {
        datasets: [{
            data: [300, 300, 300, 300, 300],
            // backgroundColor: ['#FFF','#000','#333'],
            borderWidth: 0,
        }]
    };

    public pieChartType: ChartType = 'doughnut';

    public chartClicked({ event, active }: { event: ChartEvent, active: {}[] }): void {
        console.log(event, active);
    }

    public chartHovered({ event, active }: { event: ChartEvent, active: {}[] }): void {
        console.log(event, active);
    }

	ngOnInit() {
		/**
		 * Fetches the data
		 */
		this.fetchData();
	}

	ngAfterViewInit() {}

	/**
	 * Fetches the data
	 */
	private fetchData() { }

	/**
	 * Change the layout onclick
	 * @param layout Change the layout
	 */
	changeLayout(layout: string) {
		this.eventService.broadcast("changeLayout", layout);
	}
}
