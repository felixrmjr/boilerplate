import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { DashboardsRoutingModule } from "./dashboards-routing.module";
import { UIModule } from "../../shared/ui/ui.module";
import { WidgetModule } from "../../shared/widget/widget.module";
import { NgbDropdownModule, NgbTooltipModule, NgbNavModule } from "@ng-bootstrap/ng-bootstrap";
import { NgChartsModule } from 'ng2-charts';

import { DefaultComponent } from "./default/default.component";

@NgModule({
	declarations: [DefaultComponent],
    imports: [CommonModule, FormsModule, ReactiveFormsModule, DashboardsRoutingModule, UIModule, NgbDropdownModule, NgbTooltipModule, NgbNavModule, NgChartsModule, WidgetModule],
})
export class DashboardsModule {}
