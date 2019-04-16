<template>
  <nav>
    <v-toolbar app flat>
      <v-toolbar-side-icon @click="drawer = !drawer"></v-toolbar-side-icon>
      <v-toolbar-title>Consilium</v-toolbar-title>
    </v-toolbar>

    <v-navigation-drawer app v-model='drawer'>
      <v-list>

        <!-- Avatar -->
        <v-list-tile avatar>
          <v-list-tile-avatar>
            <img src="https://randomuser.me/api/portraits/men/85.jpg">
          </v-list-tile-avatar>
          <v-list-tile-content>
            <v-list-tile-title>Marvin Zeising</v-list-tile-title>
          </v-list-tile-content>
        </v-list-tile>

        <!-- Home -->
        <v-list-tile href="/"
        >
          <v-list-tile-action>
            <v-icon>home</v-icon>
          </v-list-tile-action>
          <v-list-tile-title>Home</v-list-tile-title>
        </v-list-tile>

        <!-- Projects -->
        <v-list-group
          v-for="(project, i) in projects"
          :key="i"
          prepend-icon="extension"
          value="true"
        >
          <template v-slot:activator>
            <v-list-tile>
              <v-list-tile-title v-text="project.name"></v-list-tile-title>
            </v-list-tile>
          </template>

          <v-list-tile
            v-for="(action, j) in project.actions"
            :key="j"
            :href="'/project/' + project.id + action[2]"
          >
            <v-list-tile-action>
              <v-icon v-text="action[1]"></v-icon>
            </v-list-tile-action>
            <v-list-tile-title v-text="action[0]"></v-list-tile-title>
          </v-list-tile>

          <v-list-group
            no-action
            sub-group
          >
            <template v-slot:activator>
              <v-list-tile>
                <v-list-tile-title>Administration</v-list-tile-title>
              </v-list-tile>
            </template>

            <v-list-tile
              v-for="(action, j) in project.adminActions"
              :key="j"
              :href="'/project/' + project.id + action[2]"
            >
              <v-list-tile-action>
                <v-icon v-text="action[1]"></v-icon>
              </v-list-tile-action>
              <v-list-tile-title v-text="action[0]"></v-list-tile-title>
            </v-list-tile>
          </v-list-group>

        </v-list-group>

        <!-- Profile -->
        <v-list-group
          prepend-icon="person"
        >
          <template v-slot:activator>
            <v-list-tile>
              <v-list-tile-title>Profile</v-list-tile-title>
            </v-list-tile>
          </template>

          <v-list-tile
            v-for="(action, i) in profileActions"
            :key="i"
            :href="action[2]"
          >
            <v-list-tile-action>
              <v-icon v-text="action[1]"></v-icon>
            </v-list-tile-action>
            <v-list-tile-title v-text="action[0]"></v-list-tile-title>
          </v-list-tile>
        </v-list-group>

        <!-- Sign out -->
        <v-list-tile
          href="/sign-up"
        >
          <v-list-tile-action>
            <v-icon>exit_to_app</v-icon>
          </v-list-tile-action>
          <v-list-tile-title>Sign out</v-list-tile-title>
        </v-list-tile>
      </v-list>
    </v-navigation-drawer>
  </nav>
</template>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({
  data() {
    return {
      drawer: true,
      projects: [{
        id: 'asdf',
        name: 'Wuppertrolley',
        actions: [
          ['Knowledge Base', 'subject', '/knowledge-base'],
          ['Calendar', 'today', '/calendar']
        ],
        adminActions: [
          ['Settings', 'settings', '/settings'],
          ['User Management', 'group', '/users'],
          ['Categories', 'label', '/users'],
          ['Teams', 'supervisor_account', '/users'],
          ['Meeting Points', 'location_on', '/users'],
          ['Notifications', 'notifications', '/users'],
          ['Reports', 'message', '/users'],
          ['Statistics', 'show_chart', '/users'],
          ['Notes', 'edit', '/users']
        ]
      }],
      profileActions: [
        ['Personal', 'account_circle', '/profile/personal'],
        ['Spiritual', 'assignment_turned_in', '/profile/spritual'],
        ['Availability', 'event_available', '/profile/availability'],
        ['Account', 'lock', '/profile/account'],
        ['Notifications', 'notifications', '/profile/notifications'],
        ['Configure Projects', 'settings', '/profile/projects']
      ]
    }
  }
})
</script>
