import { Component, OnInit } from "@angular/core";
import { ElectronService } from "./core/services/electron.service";

@Component({
	selector: "app-root",
	templateUrl: "./app.component.html",
	styleUrls: ["./app.component.scss"],
})
export class AppComponent implements OnInit {
	constructor(private electronService: ElectronService) {}

	ngOnInit() {
		if (this.electronService.isElectron) {
			console.log(process.env);
			console.log("Run in electron");
			console.log("Electron ipcRenderer", this.electronService.ipcRenderer);
			console.log("NodeJS childProcess", this.electronService.childProcess);
		} else {
			console.log("Run in browser");
		}
	}
}
