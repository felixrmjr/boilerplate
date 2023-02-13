import { UserModule } from './user/user.module';
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { NgbNavModule, NgbDropdownModule, NgbModalModule, NgbTooltipModule, NgbCollapseModule } from "@ng-bootstrap/ng-bootstrap";
import { LightboxModule } from "ngx-lightbox";

import { WidgetModule } from "../shared/widget/widget.module";
import { UIModule } from "../shared/ui/ui.module";

import { PagesRoutingModule } from "./pages-routing.module";

import { DashboardsModule } from "./dashboards/dashboards.module";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";

@NgModule({
	declarations: [],
	imports: [
		CommonModule,
		FormsModule,
		NgbDropdownModule,
		NgbModalModule,
		PagesRoutingModule,
		ReactiveFormsModule,
		DashboardsModule,
        UserModule,
		HttpClientModule,
		UIModule,
		WidgetModule,
		NgbNavModule,
		NgbTooltipModule,
		NgbCollapseModule,
		LightboxModule,
	],
})
export class PagesModule {}
