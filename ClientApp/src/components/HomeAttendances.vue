<template>
  <v-flex xs12 sm10 md8 lg6 xl4>
    <v-card flat class="ma-2 mb-5" :loading="loading">
      <v-card-text class="text--primary">
        <h2 class="headline">{{ $tc('person.attendance.attendances', 2) }}</h2>
        <span class="grey--text" v-t="'person.attendance.description'" />
      </v-card-text>

      <v-list v-if="personModule.getActivePerson" two-line>
        <v-list-item
          v-if="personModule.getActivePerson.getAttendances.length === 0"
        >{{ $tc('person.attendance.attendances', 0) }}</v-list-item>

        <v-list-item
          v-for="(attendance, index) in personModule.getActivePerson.getAttendances"
          :key="index"
          @click="openShift()"
        >
          <v-list-item-content>
            <v-list-item-title>
              {{ userModule.getUser.formatDate(attendance.shift.date, 'YYYYMMDD') }}
              {{ $t('person.attendance.at') }}
              {{ attendance.shift.getTimespan(userModule.getUser) }}
            </v-list-item-title>

            <v-list-item-subtitle>
              {{ attendance.shift.category.project.name }}
              ({{ attendance.shift.category.name }})
            </v-list-item-subtitle>
          </v-list-item-content>
          <v-list-item-action>
            <v-icon>info</v-icon>
          </v-list-item-action>
        </v-list-item>
      </v-list>
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import UserModule from '../store/users'
import PersonModule from '../store/persons'
import ApplicationModule from '../store/applications'

@Component
export default class HomeAttendances extends Vue {
  private userModule = getModule(UserModule, this.$store)
  private personModule = getModule(PersonModule, this.$store)
  private applicationModule = getModule(ApplicationModule, this.$store)

  private loading = true

  @Watch('personModule.getActivePerson')
  private async onPersonChanged(val: string, oldVal: string) {
    await this.init()
  }

  private async created() {
    await this.init()
  }

  private async init() {
    this.loading = true

    await this.applicationModule.loadMyAttendances()

    this.loading = false
  }

  private openShift() {}
}
</script>
