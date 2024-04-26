import {
  CircleIcon,
  WindmillIcon,
  TypographyIcon,
  ShadowIcon,
  PaletteIcon,
  KeyIcon,
  BugIcon,
  DashboardIcon,
  BrandChromeIcon,
  HelpIcon,
  AffiliateIcon,
  UsersIcon
} from 'vue-tabler-icons';

export interface menu {
  header?: string;
  title?: string;
  icon?: object;
  to?: string;
  divider?: boolean;
  chip?: string;
  chipColor?: string;
  chipVariant?: string;
  chipIcon?: string;
  children?: menu[];
  disabled?: boolean;
  type?: string;
  subCaption?: string;
}

const AdminNavItems: menu[] = [
  {
    title: 'Worskpaces',
    icon: AffiliateIcon,
    to: '/admin/workspaces'
  },
  {
    title: 'Users',
    icon: UsersIcon,
    to: '/admin/users'
  }
];

const WorkspaceNavItems: menu[] = [
  {
    title: 'Patients',
    icon: BugIcon,
    to: '/starter'
  },
  {
    title: 'Settings',
    icon: TypographyIcon,
    to: '/starter'
  }
];

export { AdminNavItems, WorkspaceNavItems };
