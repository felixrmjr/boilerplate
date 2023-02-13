import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { UserRoutingModule } from "./user-routing.module";
import { UIModule } from "../../shared/ui/ui.module";
import { WidgetModule } from "../../shared/widget/widget.module";

import { NgbDropdownModule, NgbTooltipModule, NgbNavModule, NgbAlertModule } from "@ng-bootstrap/ng-bootstrap";
import { ProfileComponent } from "./profile/profile.component";

@NgModule({
	declarations: [ProfileComponent],
    imports: [CommonModule, FormsModule, ReactiveFormsModule, UserRoutingModule, NgbAlertModule, UIModule, NgbDropdownModule, NgbTooltipModule, NgbNavModule, WidgetModule],
})
export class UserModule {}
