import { MenuItem } from "./menu.model";

export const MENU: MenuItem[] = [
	{
		id: 1,
		label: "MENUITEMS.MENU.TEXT",
		isTitle: true,
	},
	{
		id: 2,
		label: "MENUITEMS.DASHBOARDS.TEXT",
		icon: "bx-home-circle",
		badge: {
			variant: "info",
			text: "MENUITEMS.DASHBOARDS.BADGE",
		},
		subItems: [
			{
				id: 3,
				label: "MENUITEMS.DASHBOARDS.LIST.DEFAULT",
				link: "/",
				parentId: 2,
			},
		],
	},
	{
		id: 7,
		isLayout: true,
	},
	{
		id: 8,
		label: "MENUITEMS.APPS.TEXT",
		isTitle: true,
	},
	{
		id: 9,
		label: "MENUITEMS.CHAT.TEXT",
		icon: "bx-chat",
		link: "/",
	},
];
