<template>
  <v-flex xs12 sm10 md8 lg6 xl4>
    <h2 class="headline mb-3">
      {{ $tc('person.application.applications', 2) }}
    </h2>
    <v-card
      flat
      class="ma-2 mb-5"
      :loading="loading"
    >
      <v-card-text
        class="grey--text"
        v-t="'person.application.description'"
      />
      <v-list
        v-if="personModule.getActivePerson"
        two-line
      >
        <v-list-item v-if="personModule.getActivePerson.getApplications.length === 0">
          {{ $tc('person.application.applications', 0) }}
        </v-list-item>
        <v-list-item
          v-for="(application, index) in personModule.getActivePerson.getApplications"
          :key="index"
        >
          <v-list-item-content>
            <v-list-item-title>
              {{ userModule.getUser.formatDate(application.shift.date, 'YYYYMMDD') }}
              {{ $t('person.application.at') }}
              {{ application.shift.getTimespan(userModule.getUser) }}
            </v-list-item-title>
            <v-list-item-subtitle>
              {{ application.shift.category.project.name }}
              ({{ application.shift.category.name }})
            </v-list-item-subtitle>
          </v-list-item-content>
          <v-list-item-action>
            <!-- <UpdateCategoryDialog :category="category" /> -->
          </v-list-item-action>
        </v-list-item>
      </v-list>
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
// import UpdateCategoryDialog from '../components/dialogs/UpdateCategoryDialog.vue'
import UserModule from '../store/users'
import PersonModule from '../store/persons'
import ApplicationModule from '../store/applications'

@Component({
  components: {
    // UpdateCategoryDialog,
  }
})
export default class HomeApplications extends Vue {
  private userModule = getModule(UserModule, this.$store)
  private personModule = getModule(PersonModule, this.$store)
  private applicationModule = getModule(ApplicationModule, this.$store)

  private loading: boolean = true

  @Watch('personModule.getActivePerson')
  private async onPersonChanged(val: string, oldVal: string) {
    await this.init()
  }

  private async created() {
    await this.init()
  }

  private async init() {
    this.loading = true

    await this.applicationModule.loadMyApplications()

    this.loading = false
  }

}
</script>
