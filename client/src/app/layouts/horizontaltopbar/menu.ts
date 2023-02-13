import { MenuItem } from "./menu.model";

export const MENU: MenuItem[] = [
	{
		id: 1,
		label: "MENUITEMS.DASHBOARDS.TEXT",
		icon: "bx-home-circle",
		subItems: [
			{
				id: 2,
				label: "MENUITEMS.DASHBOARDS.LIST.DEFAULT",
				link: "/dashboard",
				parentId: 1,
			},
		],
	},
	{
		id: 24,
		label: "MENUITEMS.APPS.TEXT",
		icon: "bx-customize",
		subItems: [
			{
				id: 26,
				label: "MENUITEMS.CHAT.TEXT",
				link: "/",
				parentId: 24,
			},
		],
	},
];
