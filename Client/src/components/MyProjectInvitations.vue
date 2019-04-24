<template>
  <div class="myProjectInvitations">
    <!--//* Project invitations -->
    <v-flex xs12>
      <h2 class="headline mb-3">Pending Project invitations</h2>
    </v-flex>

    <!--//* Project invitations loading -->
    <v-flex
      v-if="loading"
      class="mb-5"
    >
      <v-progress-circular
        indeterminate
        color="primary"
      />
    </v-flex>

    <!--//* No Project invitations existing -->
    <v-flex v-if="!loading && invitations == []">
      <i>You don't have any pending Project invitations.</i>
    </v-flex>

    <!--//* Data -->
    <v-flex
      v-for="(project, i) in invitations"
      :key="'invitation' + i"
      xs12 sm8 md6 lg4
      class="mb-5 pa-2"
    >
      <v-card>
        <v-card-title primary-title>
          <h3
            v-text="project.name"
            class="title mb-0 ml-2"
          />
        </v-card-title>

        <v-card-actions>
          <v-btn
            flat
            color="primary"
          >
            Accept
          </v-btn>
          <v-spacer></v-spacer>
          <v-btn
            flat
            color="error"
          >
            Decline
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-flex>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({
  data() {
    return {
      loading: false,
      invitations: [{
        name: 'Dummy Project'
      }]
    }
  }
})
</script>
