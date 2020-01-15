<template>
  <v-container
    v-if="canView"
    fluid
  >
    <v-layout wrap>

      <!--//* Arrow Left -->
      <v-flex
        xs12 sm4
        class="text-sm-left text-xs-center"
      >
        <v-btn @click.stop="$refs.calendar.prev()">
          <v-icon dark left>keyboard_arrow_left</v-icon>
          Prev
        </v-btn>
      </v-flex>

      <!--//* Calendar Type -->
      <v-flex
        xs12 sm4
        class="text-center"
      >
        <v-btn-toggle
          v-model="type"
          color="primary"
          mandatory
          borderless
        >
          <v-btn value="month">Month</v-btn>
          <v-btn value="week">Week</v-btn>
          <v-btn value="day">Day</v-btn>
        </v-btn-toggle>
      </v-flex>

      <!--//* Arrow Right -->
      <v-flex
        xs12 sm4
        class="text-sm-right text-xs-center"
      >
        <v-btn @click.stop="$refs.calendar.next()">
          Next
          <v-icon right dark>keyboard_arrow_right</v-icon>
        </v-btn>
      </v-flex>

      <!--//* Calendar -->
      <v-row>
        <v-flex
          xs12
          class="mt-3"
          style="margin-right:1px;"
        >
          <v-sheet>
            <v-calendar
              ref="calendar"
              color="primary"
              v-model="focus"
              :events="events"
              :type="type"
              :weekdays="weekdays"
              @click:event="showEvent"
              @click:more="viewDay"
            >
              <template v-slot:day-label="date">
                <v-menu>
                  <template v-slot:activator="{ on }">
                    <v-btn
                      v-on="on"
                      fab
                      small
                      class="mb-1"
                      :icon="date.present ? false : true"
                      :color="date.present ? 'primary' : ''"
                      v-text="date.day"
                      rounded
                      depressed
                      @click="canEdit ? false : viewDay(date.date)"
                    />
                  </template>
                  <v-list>
                    <v-list-item @click="viewDay">
                      <v-list-item-title>Go to day</v-list-item-title>
                    </v-list-item>
                    <v-list-item>
                      <v-list-item-title>Create shift</v-list-item-title>
                    </v-list-item>
                  </v-list>
                </v-menu>

              </template>
            </v-calendar>
          </v-sheet>
        </v-flex>
      </v-row>

    </v-layout>
  </v-container>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import moment from 'moment'
import PersonModule from '../../store/persons'
import ProjectModule from '../../store/projects'

@Component
export default class Calendar extends Vue {
  private personModule: PersonModule = getModule(PersonModule, this.$store)
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  private type: string = 'month'
  private focus: string = moment().format('YYYY-MM-DD')
  private events: any = []
  private weekdays: number[] = [1, 2, 3, 4, 5, 6, 0]

  public get canView() {
    return this.personModule.getActiveRole?.calendarRead === true
  }

  private get canEdit() {
    return this.personModule.getActiveRole?.calendarWrite === true
  }

  private created() {
    this.events.push({
      name: 'Shift',
      start: moment().format('YYYY-MM-DD hh:mm'),
      end: moment().add(2, 'hours').format('YYYY-MM-DD hh:mm'),
      color: 'primary',
    })
  }

  private viewDay(date: any) {
    this.focus = date
    this.type = 'day'
  }

}
</script>
