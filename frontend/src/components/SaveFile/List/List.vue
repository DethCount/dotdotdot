<template>
    <div class="row" v-if="showActions">
        <router-link
            v-if="showDiffBtn"
            class="btn btn-info"
            v-bind:to="{
                name: 'SaveFileDiff',
                params: {
                    filename1: firstSelectedRow,
                    filename2: secondSelectedRow
                }
            }"
            >View Diff</router-link>
    </div>
    <div class="row">
        <table v-if="loaded" class="table table-striped">
            <thead>
                <th>Path</th>
                <th>Last Modified</th>
            </thead>
            <tbody>
                <tr
                    v-bind:key="item.filename"
                    v-for="item in items"
                    v-on:click="onRowClick(item)"
                    v-bind:class="{
                        'selected-first': isSelectedFirst(item),
                        'selected-second': isSelectedSecond(item),
                    }"
                    >
                    <td><router-link v-bind:to="{name: 'SaveFileRead', params: { filename: item.filename }}">{{ item.filename }}</router-link></td>
                    <td>{{ item.lastModified }}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script src="@/components/SaveFile/List/List.js"></script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="sass" src="@/components/SaveFile/List/List.sass"></style>
