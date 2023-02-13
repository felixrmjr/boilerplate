import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { NgbModalModule } from "@ng-bootstrap/ng-bootstrap";

import { StatComponent } from "./stat/stat.component";

@NgModule({
	declarations: [StatComponent],
	imports: [CommonModule, NgbModalModule],
	exports: [StatComponent],
})
export class WidgetModule {}
